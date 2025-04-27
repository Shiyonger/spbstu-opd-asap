package com.asap.opdasap.event;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class GitHubPullRequestEvent {

    @JsonProperty("action")
    private String action;

    @JsonProperty("pull_request")
    private PullRequest pullRequest;

    @Getter
    @Setter
    public static class PullRequest {
        @JsonProperty("title")
        private String title;

        @JsonProperty("state")
        private String state;
    }
}
