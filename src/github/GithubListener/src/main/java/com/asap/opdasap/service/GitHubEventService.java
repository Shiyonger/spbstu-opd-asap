package com.asap.opdasap.service;

import com.asap.opdasap.KafkaPublisher;
import com.asap.opdasap.MessageAction;
import com.asap.opdasap.MessagePoints;
import com.asap.opdasap.event.GitHubPullRequestEvent;
import com.asap.opdasap.event.GitHubPushEvent;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

import java.util.Date;

@Slf4j
@Service
@RequiredArgsConstructor
public class GitHubEventService {

    private final ObjectMapper objectMapper;
    private final GitHubCommentParserService commentParserService;
    private final KafkaPublisher kafkaPublisher;

    public MessageAction handlePushEvent(String payload) throws Exception {
        GitHubPushEvent event = objectMapper.readValue(payload, GitHubPushEvent.class);

        String repoName = event.getRepository().getName();
        String owner = event.getRepository().getOwner().getLogin();

        MessageAction action = new MessageAction();
        action.setUsername(owner);
        action.setAssignmentTitle(repoName);
        action.setDate(new Date());
        action.setAction(MessageAction.ActionType.UPDATE); // Для push события используем "Update"

        kafkaPublisher.publish("action", action);
        return action;
    }

    public MessageAction handlePullRequestEvent(String payload) throws Exception {
        GitHubPullRequestEvent event = objectMapper.readValue(payload, GitHubPullRequestEvent.class);

        String actionType = event.getAction();
        GitHubPullRequestEvent.PullRequest pr = event.getPullRequest();

        if (pr == null || pr.getBase() == null || pr.getBase().getRepo() == null) {
            log.warn("PR или его базовые данные отсутствуют");
            return null;
        }

        String nickname = pr.getUser().getLogin();
        String repo = pr.getBase().getRepo().getName();
        String owner = pr.getBase().getRepo().getOwner().getLogin();
        int prNumber = pr.getNumber();

        MessageAction action = new MessageAction();
        action.setUsername(nickname);
        action.setAssignmentTitle(repo);
        action.setDate(new Date());
        action.setAction(mapActionType(actionType));

        kafkaPublisher.publish("action", action);

        if ("closed".equalsIgnoreCase(actionType)) {
            try {
                MessagePoints points = commentParserService.extractPointsFromPullRequestComments(
                        owner, repo, prNumber, nickname, repo
                );

                if (points != null) {
                    kafkaPublisher.publish("points", points);
                } else {
                    log.warn("Баллы не найдены в комментариях для PR #{} в {}/{}", prNumber, owner, repo);
                }
            } catch (Exception e) {
                log.error("Ошибка при извлечении/отправке баллов для PR #" + prNumber, e);
            }
        }

        return action;
    }

    private MessageAction.ActionType mapActionType(String action) {
        return switch (action.toLowerCase()) {
            case "opened" -> MessageAction.ActionType.CREATE;
            case "synchronize", "reopened", "edited" -> MessageAction.ActionType.UPDATE;
            case "closed" -> MessageAction.ActionType.DELETE;
            default -> MessageAction.ActionType.UPDATE;
        };
    }
}