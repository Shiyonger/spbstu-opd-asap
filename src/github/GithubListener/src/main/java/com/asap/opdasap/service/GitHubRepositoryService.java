package com.asap.opdasap.service;


import com.asap.opdasap.RepositoryInfo;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.List;
import java.util.stream.Collectors;

@Slf4j
@Service
@RequiredArgsConstructor
public class GitHubRepositoryService {

    private final RestTemplate restTemplate = new RestTemplate();

    @Value("${github.token}")
    private String githubToken;

    @Value("${github.api.url:https://api.github.com}")
    private String githubApiUrl;

    @Value("${github.template.repo.owner}")
    private String templateOwner;

    @Value("${github.template.repo.name}")
    private String templateRepoName;

    public List<RepositoryInfo> createRepositories(List<String> usernames, String org, String assignmentTitle) {
        return usernames.stream().map(username -> {
            try {
                String repoName = username + "-" + assignmentTitle.replaceAll("\\s+", "-");

                String url = githubApiUrl + "/repos/" + templateOwner + "/" + templateRepoName + "/generate";

                HttpHeaders headers = new HttpHeaders();
                headers.setBearerAuth(githubToken);
                headers.setAccept(List.of(MediaType.APPLICATION_JSON));
                headers.setContentType(MediaType.APPLICATION_JSON);

                String requestBody = String.format("""
                        {
                          "owner": "%s",
                          "name": "%s",
                          "private": true
                        }
                        """, org, repoName);

                HttpEntity<String> request = new HttpEntity<>(requestBody, headers);

                ResponseEntity<String> response = restTemplate.postForEntity(url, request, String.class);

                if (response.getStatusCode().is2xxSuccessful()) {
                    String repoUrl = "https://github.com/" + org + "/" + repoName;
                    log.info("✅ Репозиторий создан: {}", repoUrl);
                    return new RepositoryInfo(username, org, assignmentTitle, repoUrl);
                } else {
                    log.error("❌ Ошибка создания репозитория для {}: {}", username, response.getBody());
                    return null;
                }

            } catch (Exception e) {
                log.error("❌ Исключение при создании репозитория для {}: {}", username, e.getMessage());
                return null;
            }
        }).filter(r -> r != null).collect(Collectors.toList());
    }
}