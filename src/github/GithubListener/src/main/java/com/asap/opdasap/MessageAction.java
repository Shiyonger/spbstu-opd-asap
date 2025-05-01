package com.asap.opdasap;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.io.Serializable;
import java.util.Date;

public final class MessageAction implements Serializable {

    @JsonProperty("nickname")
    public String nickname;

    @JsonProperty("date")
    public Date date;

    @JsonProperty("assignment_name")
    public String assignmentName;   // Название репозитория

    // Посмотреть можем ли понять создан или обновлен pull request

}
