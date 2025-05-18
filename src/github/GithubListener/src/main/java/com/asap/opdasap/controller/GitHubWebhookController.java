package com.asap.opdasap.controller;

import com.asap.opdasap.security.GitHubSignatureVerifier;
import com.asap.opdasap.service.GitHubEventService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/github/webhook")
public class GitHubWebhookController {

    private static final Logger logger = LoggerFactory.getLogger(GitHubWebhookController.class);

    private final GitHubEventService eventService;
    private final GitHubSignatureVerifier signatureVerifier;

    public GitHubWebhookController(GitHubEventService eventService, GitHubSignatureVerifier signatureVerifier) {
        this.eventService = eventService;
        this.signatureVerifier = signatureVerifier;
    }

    @PostMapping
    public ResponseEntity<?> handleWebhook(
            @RequestBody String payload,
            @RequestHeader("X-GitHub-Event") String eventType,
            @RequestHeader(value = "X-Hub-Signature-256", required = false) String signature
    ) {
        logger.info("Received GitHub event: {}", eventType);
        if (signature != null && !signatureVerifier.verifySignature(payload, signature)) {
            logger.warn("Invalid GitHub signature");
            return ResponseEntity.status(401).body("Invalid signature");
        }

        try {
            Object response = switch (eventType) {
                case "push" -> eventService.handlePushEvent(payload);
                case "pull_request" -> eventService.handlePullRequestEvent(payload);
                default -> throw new IllegalArgumentException("Unsupported event type: " + eventType);
            };
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            logger.error("Error processing GitHub event", e);
            return ResponseEntity.internalServerError().body("Error: " + e.getMessage());
        }
    }
}
