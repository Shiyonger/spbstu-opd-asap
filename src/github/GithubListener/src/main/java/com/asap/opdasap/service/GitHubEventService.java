package com.asap.opdasap.service;

import com.asap.opdasap.event.GitHubPushEvent;
import com.asap.opdasap.event.GitHubPullRequestEvent;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.stereotype.Service;

@Service
public class GitHubEventService {

    private final ObjectMapper objectMapper = new ObjectMapper();

    public String handlePushEvent(String payload) throws Exception {
        GitHubPushEvent event = objectMapper.readValue(payload, GitHubPushEvent.class);
        return "Push to branch '" + event.getRef() + "' in repo: " + event.getRepository().getName();
    }

    public String handlePullRequestEvent(String payload) throws Exception {
        GitHubPullRequestEvent event = objectMapper.readValue(payload, GitHubPullRequestEvent.class);
        return "PR Action: " + event.getAction() + ", Title: " + event.getPullRequest().getTitle();
    }
}
