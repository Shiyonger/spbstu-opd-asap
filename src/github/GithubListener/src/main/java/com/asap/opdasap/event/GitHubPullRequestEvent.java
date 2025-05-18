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

        @JsonProperty("number")
        private int number;

        @JsonProperty("user")
        private User user;

        @JsonProperty("base")
        private Base base;
    }

    @Getter
    @Setter
    public static class User {
        @JsonProperty("login")
        private String login;
    }

    @Getter
    @Setter
    public static class Base {
        @JsonProperty("repo")
        private Repository repo;
    }

    @Getter
    @Setter
    public static class Repository {
        @JsonProperty("name")
        private String name;

        @JsonProperty("owner")
        private Owner owner;
    }

    @Getter
    @Setter
    public static class Owner {
        @JsonProperty("login")
        private String login;
    }
}