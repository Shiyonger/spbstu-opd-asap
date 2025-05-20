package com.asap.opdasap;

import com.asap.opdasap.service.GitHubRepositoryService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

import java.util.List;

@Slf4j
@Component
@RequiredArgsConstructor
public class GithubOrchestrationJob {

    private final GithubGrpcClient githubGrpcClient;
    private final GitHubRepositoryService gitHubRepositoryService; // для создания репозиториев

    @Scheduled(cron = "0 0 2 * * *") // каждый день в 2:00 ночи
    public void runDailyGithubSync() {
        log.info("🚀 Запуск фоновой задачи GithubOrchestrationJob");
        List<String> organizations = githubGrpcClient.getOrganizations();

        for (String org : organizations) {
            // 1. Получение пользователей для инвайта
            List<String> usersToInvite = githubGrpcClient.getUsersToInvite(org);
            log.info("🔍 Получены {} пользователей для инвайта", usersToInvite.size());

            if (!usersToInvite.isEmpty()) {
                githubGrpcClient.markInvited(usersToInvite);
                log.info("✅ Помечены как приглашённые: {}", usersToInvite);
            }

            // 2. Получение пользователей для создания репозиториев
            List<String> usersToCreateRepos = githubGrpcClient.getUsersToCreateRepositories(org, "Assignment 1");
            log.info("📦 Пользователей для создания репозиториев: {}", usersToCreateRepos.size());

            // 3. Создание репозиториев
            List<RepositoryInfo> createdRepos = gitHubRepositoryService.createRepositories(usersToCreateRepos, org, "Assignment 1");
            log.info("✅ Репозиториев создано: {}", createdRepos.size());

            // 4. Отправка результатов через gRPC
            githubGrpcClient.sendCreatedRepositories(createdRepos);
            log.info("📬 Отправлены ссылки на созданные репозитории");
        }
    }
}