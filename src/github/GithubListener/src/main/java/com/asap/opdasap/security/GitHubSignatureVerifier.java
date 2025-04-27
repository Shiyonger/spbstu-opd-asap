package com.asap.opdasap.security;

import org.apache.commons.codec.binary.Hex;
import org.springframework.stereotype.Component;
import org.springframework.beans.factory.annotation.Value;

import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;

@Component
public class GitHubSignatureVerifier {

    private final String secret;

    public GitHubSignatureVerifier(@Value("${github.webhook.secret}") String secret) {
        this.secret = secret;
    }

    private static final String HMAC_ALGO = "HmacSHA256";

    public boolean verifySignature(String payload, String signature) {
        try {
            SecretKeySpec secretKeySpec = new SecretKeySpec(secret.getBytes(), HMAC_ALGO);
            Mac mac = Mac.getInstance(HMAC_ALGO);
            mac.init(secretKeySpec);
            byte[] hmac = mac.doFinal(payload.getBytes());
            String computed = "sha256=" + Hex.encodeHexString(hmac);
            return computed.equals(signature);
        } catch (Exception e) {
            return false;
        }
    }
}
