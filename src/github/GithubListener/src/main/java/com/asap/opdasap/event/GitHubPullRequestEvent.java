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

    public String getAction() {
        return action;
    }

    public void setAction(String action) {
        this.action = action;
    }

    public PullRequest getPullRequest() {
        return pullRequest;
    }

    public void setPullRequest(PullRequest pullRequest) {
        this.pullRequest = pullRequest;
    }

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

        public String getTitle() {
            return title;
        }

        public void setTitle(String title) {
            this.title = title;
        }

        public String getState() {
            return state;
        }

        public void setState(String state) {
            this.state = state;
        }

        public int getNumber() {
            return number;
        }

        public void setNumber(int number) {
            this.number = number;
        }

        public User getUser() {
            return user;
        }

        public void setUser(User user) {
            this.user = user;
        }

        public Base getBase() {
            return base;
        }

        public void setBase(Base base) {
            this.base = base;
        }
    }

    @Getter
    @Setter
    public static class User {
        @JsonProperty("login")
        private String login;

        public String getLogin() {
            return login;
        }

        public void setLogin(String login) {
            this.login = login;
        }
    }

    @Getter
    @Setter
    public static class Base {
        @JsonProperty("repo")
        private Repository repo;

        public Repository getRepo() {
            return repo;
        }

        public void setRepo(Repository repo) {
            this.repo = repo;
        }
    }

    @Getter
    @Setter
    public static class Repository {
        @JsonProperty("name")
        private String name;

        @JsonProperty("owner")
        private Owner owner;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public Owner getOwner() {
            return owner;
        }

        public void setOwner(Owner owner) {
            this.owner = owner;
        }
    }

    @Getter
    @Setter
    public static class Owner {
        @JsonProperty("login")
        private String login;

        public String getLogin() {
            return login;
        }

        public void setLogin(String login) {
            this.login = login;
        }
    }
}