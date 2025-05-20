package com.asap.opdasap.controller;

import com.asap.opdasap.service.GitHubOrganizationService;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

@RestController
@RequestMapping("/organization")
@RequiredArgsConstructor
public class OrganizationController {
    private static final Logger logger = LoggerFactory.getLogger(OrganizationController.class);

    private final GitHubOrganizationService gitHubOrganizationService;

    @PostMapping("/invite")
    public ResponseEntity<String> inviteUsersAndCreateRepos(@RequestBody List<String> usernames) {
        List<String> successList = new ArrayList<>();
        List<String> failedList = new ArrayList<>();

        for (String username : usernames) {
            try {
                logger.info("Processing user: {}", username);

                // Задержка между запросами (1-2 секунды)
                int delay = 1000 + (int) (Math.random() * 1000); // 1000 - 2000 мс
                TimeUnit.MILLISECONDS.sleep(delay);
                logger.info("Sleeping for {} ms", delay);

                gitHubOrganizationService.inviteUserToOrganization(username);
                gitHubOrganizationService.createRepositoryForUser(username);
                gitHubOrganizationService.addUserToRepository(username);

                successList.add(username);
            } catch (Exception e) {
                logger.error("Error processing user: {}", username, e);
                failedList.add(username + ": " + e.getMessage());
            }
        }

        StringBuilder result = new StringBuilder();
        result.append("✅ Success:\n");
        successList.forEach(user -> result.append("- ").append(user).append("\n"));

        result.append("\n\uD83D\uDED1 Failed:\n");
        failedList.forEach(error -> result.append("- ").append(error).append("\n"));

        return ResponseEntity.ok(result.toString());
    }
}