package com.asap.opdasap.service;

import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.*;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.Map;

@Service
@RequiredArgsConstructor
public class GitHubOrganizationService {

    private final RestTemplate restTemplate = new RestTemplate();

    @Value("${github.token}")
    private String githubToken;

    @Value("${github.organization}")
    private String organization;

    @Value("${github.api.url:https://api.github.com}")
    private String githubApiUrl;

    private HttpHeaders createHeaders() {
        HttpHeaders headers = new HttpHeaders();
        headers.setBearerAuth(githubToken);
        headers.setContentType(MediaType.APPLICATION_JSON);
        return headers;
    }

    public void inviteUserToOrganization(String username) {
        String url = githubApiUrl + "/orgs/" + organization + "/invitations";

        Map<String, Object> body = Map.of(
                "invitee", Map.of("login", username),
                "role", "direct_member"
        );

        HttpEntity<Map<String, Object>> request = new HttpEntity<>(body, createHeaders());

        restTemplate.exchange(url, HttpMethod.POST, request, Void.class);
    }

    public void createRepositoryForUser(String username) {
        String repoName = username + "-repo";
        String url = githubApiUrl + "/orgs/" + organization + "/repos";

        Map<String, Object> body = Map.of(
                "name", repoName,
                "private", true
        );

        HttpEntity<Map<String, Object>> request = new HttpEntity<>(body, createHeaders());

        restTemplate.exchange(url, HttpMethod.POST, request, Void.class);
    }

    public void addUserToRepository(String username) {
        String repoName = username + "-repo";
        String url = githubApiUrl + "/repos/" + organization + "/" + repoName + "/collaborators/" + username;

        Map<String, Object> body = Map.of(
                "permission", "push"
        );

        HttpEntity<Map<String, Object>> request = new HttpEntity<>(body, createHeaders());

        restTemplate.exchange(url, HttpMethod.PUT, request, Void.class);
    }
}
