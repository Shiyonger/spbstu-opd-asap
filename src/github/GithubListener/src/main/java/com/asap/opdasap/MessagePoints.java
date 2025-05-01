package com.asap.opdasap;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.io.Serializable;
import java.util.Date;

public final class MessagePoints implements Serializable {

    @JsonProperty("assignment_name")
    public String assignmentName;   // Название репозитория

    @JsonProperty("nickname")
    public String nickname;

    @JsonProperty("date")
    public Date date;

    @JsonProperty("points")
    public int points;

}
