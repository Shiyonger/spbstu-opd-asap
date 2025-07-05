package com.asap.opdasap.service;

import com.asap.opdasap.MessagePoints;
import com.fasterxml.jackson.databind.JsonNode;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.*;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.Date;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

@Slf4j
@Service
@RequiredArgsConstructor
public class GitHubCommentParserService {

    private final RestTemplate restTemplate = new RestTemplate();

    @Value("${github.token}")
    private String githubToken;

    @Value("${github.api.url:https://api.github.com}")
    private String githubApiUrl;

    private final String courseTitle = "opd-asap-test";

    public MessagePoints extractPointsFromPullRequestComments(
            String owner,
            String repo,
            int prNumber,
            String nickname,
            String assignmentName
    ) {
        String url = String.format("%s/repos/%s/%s/issues/%d/comments", githubApiUrl, owner, repo, prNumber);

        HttpHeaders headers = new HttpHeaders();
        headers.setBearerAuth(githubToken);
        headers.setAccept(List.of(MediaType.APPLICATION_JSON));
        HttpEntity<Void> request = new HttpEntity<>(headers);

        ResponseEntity<JsonNode> response = restTemplate.exchange(url, HttpMethod.GET, request, JsonNode.class);

        if (!response.getStatusCode().is2xxSuccessful() || response.getBody() == null) {
            log.warn("Не удалось получить комментарии по PR #{}", prNumber);
            return null;
        }

        for (JsonNode comment : response.getBody()) {
            String body = comment.get("body").asText();
            Matcher matcher = Pattern.compile("/rate\\s+(-?\\d{1,4})").matcher(body); // Поддержка отрицательных чисел
            if (matcher.find()) {
                int rawPoints = Integer.parseInt(matcher.group(1));
                int points = Math.max(0, Math.min(rawPoints, 100)); // Нормализация: [0; 100]

                if (points != rawPoints) {
                    log.warn("Обрезка баллов: указано {}, приведено к {}", rawPoints, points);
                }

                MessagePoints result = new MessagePoints();
                result.setPoints(points);
                result.setAssignmentTitle(assignmentName);
                result.setCourseTitle(courseTitle);
                result.setUsername(nickname);

                return result;
            }
        }

        log.info("Не найден комментарий с баллами для PR #{}", prNumber);
        return null;
    }
}