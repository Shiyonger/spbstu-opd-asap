package com.asap.opdasap.event;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class GitHubPushEvent {

    @JsonProperty("ref")
    private String ref;

    @JsonProperty("repository")
    private Repository repository;

    public String getRef() {
        return ref;
    }

    public void setRef(String ref) {
        this.ref = ref;
    }

    public Repository getRepository() {
        return repository;
    }

    public void setRepository(Repository repository) {
        this.repository = repository;
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
