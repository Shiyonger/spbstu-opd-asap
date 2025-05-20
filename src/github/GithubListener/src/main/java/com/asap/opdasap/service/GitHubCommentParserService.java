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

    @Value("${google.sheet.id}") // spreadsheetId из настроек
    private String spreadsheetId;

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
            Matcher matcher = Pattern.compile("/rate \\b(\\d{1,3})\\b").matcher(body); // просто число, например "21"
           // todo баллы меньше 0 и больше 100
            if (matcher.find()) {
                int points = Integer.parseInt(matcher.group(1));
                MessagePoints result = new MessagePoints();
                result.setPoints(points);
                result.setDate(new Date());
                result.setAssignmentName(assignmentName); // repo
                result.setCourseTitle(owner);             // organization

                MessagePoints.Position studentPos = new MessagePoints.Position();
                studentPos.setCell(nickname);
                studentPos.setSpreadsheetId(spreadsheetId);

                MessagePoints.Position assignmentPos = new MessagePoints.Position();
                assignmentPos.setCell(assignmentName);
                assignmentPos.setSpreadsheetId(spreadsheetId);

                result.setStudentPosition(studentPos);
                result.setAssignmentPosition(assignmentPos);

                return result;
            }
        }

        log.info("Не найден комментарий с баллами для PR #{}", prNumber);
        return null;
    }
}