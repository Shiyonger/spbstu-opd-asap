package com.asap.opdasap;

import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class RepositoryInfo {
    private String username;
    private String organization;
    private String assignmentTitle;
    private String repositoryUrl;
}