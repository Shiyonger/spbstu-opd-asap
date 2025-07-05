package com.asap.opdasap;

import io.grpc.ManagedChannel;
import io.grpc.ManagedChannelBuilder;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.*;

import com.asap.opdasap.grpc.GithubServiceGrpc;




@Configuration
public class GrpcClientConfig {

    @Value("${grpc.server.address}")
    private String grpcServerAddress;

    @Bean
    public GithubServiceGrpc.GithubServiceBlockingStub githubServiceStub() {
        ManagedChannel channel = ManagedChannelBuilder
                .forTarget(grpcServerAddress)
                .usePlaintext()
                .build();

        return GithubServiceGrpc.newBlockingStub(channel);
    }
}
