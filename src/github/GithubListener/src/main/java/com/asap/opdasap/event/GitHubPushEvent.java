package com.asap.opdasap.event;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
// Добавить подобные классы для событий
public class GitHubPushEvent {

    @JsonProperty("ref")
    private String ref;  // Например, "refs/heads/main"

    @JsonProperty("repository")
    private Repository repository;


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
