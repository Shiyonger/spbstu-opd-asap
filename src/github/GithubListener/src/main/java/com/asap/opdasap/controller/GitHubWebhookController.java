package com.asap.opdasap.controller;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;


@RestController
@RequestMapping("/github/webhook")
public class GitHubWebhookController {
    private static final Logger logger = LoggerFactory.getLogger(GitHubWebhookController.class);
    //private static final String SECRET = "your-secret-key";

    @PostMapping
    public ResponseEntity<String> handleWebhook(
            @RequestBody String payload,
            @RequestHeader("X-GitHub-Event") String eventType,
            @RequestHeader(value = "X-Hub-Signature-256", required = false) String signature
    ) {
        logger.info("Received GitHub event: {}", eventType);

        // 1. Проверка подписи
//        if (signature == null) {
//            logger.warn("Missing signature");
//            return ResponseEntity.status(401).body("Missing signature");
//        }
//        if (!isValidSignature(payload, signature, SECRET)) {
//            logger.warn("Invalid signature");
//            return ResponseEntity.status(401).body("Invalid signature");
//        }

        // 2. Обработка события
        try {
            String response = switch (eventType) {
                case "push" -> handlePushEvent(payload);
                case "pull_request" -> handlePullRequestEvent(payload);
                default -> throw new IllegalArgumentException("Unsupported event");
            };
            return ResponseEntity.ok(response);
        } catch (JsonProcessingException e) {
            logger.error("JSON parsing error", e);
            return ResponseEntity.badRequest().body("Invalid JSON");
        } catch (Exception e) {
            logger.error("Processing error", e);
            return ResponseEntity.internalServerError().body("Server error");
        }
    }

//    private boolean isValidSignature(String payload, String signature, String secret) {
//        // ... реализация проверки подписи
//    }

    private String handlePushEvent(String payload) throws JsonProcessingException {
        ObjectMapper mapper = new ObjectMapper();
        JsonNode rootNode = mapper.readTree(payload);
        String branch = rootNode.get("ref").asText();
        String repoName = rootNode.get("repository").get("name").asText();
        return "Push to branch '" + branch + "' in repo: " + repoName;
    }

    private String handlePullRequestEvent(String payload) throws JsonProcessingException {
        ObjectMapper mapper = new ObjectMapper();
        JsonNode rootNode = mapper.readTree(payload);
        String action = rootNode.get("action").asText();
        String prTitle = rootNode.get("pull_request").get("title").asText();
        return "PR Action: " + action + ", Title: " + prTitle;
    }
}

//@RestController
//@RequestMapping("/github/webhook") // Эндпоинт для вебхуков
//public class GitHubWebhookController {
//
//    @PostMapping
//    public String handleWebhook(
//            @RequestBody String payload,  // Тело запроса (JSON)
//            @RequestHeader("X-GitHub-Event") String eventType//,  // Тип события
//            //@RequestHeader(value = "X-Hub-Signature-256", required = false) String signature
//    ) {
//        // 1. Проверка подписи (если используется секрет)
////        if (signature != null && !isValidSignature(payload, signature, "your-secret-key")) {
////            throw new SecurityException("Invalid signature");
////        }
//
//        // 2. Обработка события
//        switch (eventType) {
//            case "push":
//                return handlePushEvent(payload);
//            case "pull_request":
//                return handlePullRequestEvent(payload);
//            default:
//                return "Unsupported event: " + eventType;
//        }
//    }
//
////    private boolean isValidSignature(String payload, String signature, String secret) {
////        // Реализация проверки подписи (см. ниже)
////    }
//
//
//    private String handlePullRequestEvent(String payload) {
//        // Парсинг и обработка pull_request
//        try {
//            ObjectMapper mapper = new ObjectMapper();
//            JsonNode rootNode = mapper.readTree(payload);
//            String action = rootNode.get("action").asText();  // "opened", "closed"
//            String prTitle = rootNode.get("pull_request").get("title").asText();
//            return "PR Action: " + action + ", Title: " + prTitle;
//        } catch (Exception e) {
//            return "Error parsing PR event";
//        }
//    }
//
//    // Приватный метод для обработки события 'push'
//    private String handlePushEvent(String payload) {
//        try {
//            ObjectMapper mapper = new ObjectMapper();
//            JsonNode rootNode = mapper.readTree(payload);
//            String branch = rootNode.get("ref").asText();  // Например: "refs/heads/main"
//            String repoName = rootNode.get("repository").get("name").asText();
//            return "Push to branch '" + branch + "' in repo: " + repoName;
//        } catch (Exception e) {
//            return "Error parsing push event: " + e.getMessage();
//        }
//    }
//
//
//}
