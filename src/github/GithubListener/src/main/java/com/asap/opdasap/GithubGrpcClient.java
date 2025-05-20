package com.asap.opdasap;


import com.asap.opdasap.grpc.*;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.stream.Collectors;

@Component
@RequiredArgsConstructor
public class GithubGrpcClient {

    private final GithubServiceGrpc.GithubServiceBlockingStub stub;

    public List<String> getUsersToInvite(String orgName) {
        var response = stub.getUsersToInvite(
                GetUsersToInviteRequest.newBuilder()
                        .setGithubOrganizationName(orgName)
                        .build()
        );
        return response.getUsernamesList();
    }

    public List<String> getOrganizations() {
        var response = stub.getOrganizations(
                GetOrganizationsRequest.newBuilder()
                        .build()
        );
        return response.getOrganizationsList();
    }

    public void markInvited(List<String> usernames) {
        stub.markInvited(
                MarkInvitedRequest.newBuilder()
                        .addAllUsernames(usernames)
                        .build()
        );
    }

    public List<String> getUsersToCreateRepositories(String orgName, String assignmentTitle) {
        var response = stub.getUsersToCreateRepository(
                GetUsersToCreateRepositoryRequest.newBuilder()
                        .setGithubOrganizationName(orgName)
                        .setAssignmentTitle(assignmentTitle)
                        .build()
        );
        return response.getUsernamesList();
    }

    public void sendCreatedRepositories(List<RepositoryInfo> repositories) {
        var request = CreateRepositoriesRequest.newBuilder()
                .addAllRepositories(repositories.stream().map(repo ->
                        Repository.newBuilder()
                                .setUsername(repo.getUsername())
                                .setGithubOrganizationName(repo.getOrganization())
                                .setAssignmentTitle(repo.getAssignmentTitle())
                                .setRepositoryLink(repo.getRepositoryUrl())
                                .build()
                ).collect(Collectors.toList()))
                .build();

        stub.createRepositories(request);
    }
}