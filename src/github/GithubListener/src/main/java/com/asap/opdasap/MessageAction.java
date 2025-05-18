package com.asap.opdasap;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.io.Serializable;
import java.util.Date;

public final class MessageAction implements Serializable {

    @JsonProperty("username")
    private String username;

    @JsonProperty("date")
    private Date date;

    @JsonProperty("assignment_title")
    private String assignmentTitle;

    @JsonProperty("action")
    private ActionType action;

    public enum ActionType {
        @JsonProperty("Create") CREATE,
        @JsonProperty("Update") UPDATE,
        @JsonProperty("Delete") DELETE
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public Date getDate() {
        return date;
    }

    public void setDate(Date date) {
        this.date = date;
    }

    public String getAssignmentTitle() {
        return assignmentTitle;
    }

    public void setAssignmentTitle(String assignmentTitle) {
        this.assignmentTitle = assignmentTitle;
    }

    public ActionType getAction() {
        return action;
    }

    public void setAction(ActionType action) {
        this.action = action;
    }
}

