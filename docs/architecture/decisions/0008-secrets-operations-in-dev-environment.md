# 8. secrets operations in dev environment

Date: 2025-03-04

## Status

Rejected

## Context

This proposal is a contigency in case developers cannot have access to the Azure key vault and describe Secret Operations with a Cloud Native Coumputing Foundation (CNCF) tool called [SOPS](cncf_sops) for local development

### What is SOPS

> sops is an editor of encrypted files that supports YAML, JSON, ENV, INI and BINARY formats and encrypts with AWS KMS, GCP KMS, Azure Key Vault, age, and PGP.

From CNCF

SOPS allows developers to store encrypted secrets in configuration files (e.g., YAML, JSON) alongside their application code. These encrypted files can be safely committed to version control.

Can also be integrated with various key management systems (KMS) like AWS KMS, Google Cloud KMS, HashiCorp Vault, and PGP, allowing for secure storage and management of encryption keys.

### Example

In the root folder of the monorepo we have a `.sops.yaml` manifest that sets what encryption keys we can use, on the below case we are using PGP, the first one is the UID for Pedro and the second is Aaron's

```yaml
creation_rules:
  - pgp: 781EE01BD4ACD521F47622CB7DAE7D1D01032CD8,B9B825890A6505725A738356D3B4CAEFF79C866D
```

We created a folder to store secrets `secrets/`, inside we have a file called `secrets.encripted.yaml` that is on version control

```yaml
api-key: ENC[AES256_GCM,data:c6F9ah2HVlPmNB/x,iv:nSOlGmUl4whu2ligB4MSCXYS2xWh7rDHZxt8LwO6ris=,tag:WXXW+yfgjwL03KvpFaZftg==,type:str]
sops:
  kms: []
  gcp_kms: []
  azure_kv: []
  hc_vault: []
  age: []
  lastmodified: '2025-02-17T11:20:24Z'
  mac: ENC[AES256_GCM,data:ak5w6JCDCVqaES1gzW44ZESD6rrz0KeejwJcPudQCnI644D6PV79Mc76A549sF9vjyQ3TRQsvWXN+oy6YSVEr0NOTeT3ykBGlM4+UMsVhwlgfm0UBgHuTpjsLqCwnDD/h8FpV0O7MOvHgJWQVNfcu4ExM0MKCtzQqLKTXI+72QU=,iv:CdNN06RX2m2hng4Mlrswd3D16/wey6lMrly7r3Kkf+8=,tag:ARcrtUaDOcjX2Foq4vCwxw==,type:str]
  pgp:
    - created_at: '2025-02-17T11:33:11Z'
      enc: |-
        -----BEGIN PGP MESSAGE-----

        hQGMA3T1RuKHqWsnAQv/YQmDF1G0dhiM9TOJTzYD+8EE4mLo6O4ebkjzfma2AKKs
        JSZo/nXZ2RCjQOSG3ZRc8C9paycXlHMW11MGHIwUSv7jDNkeusECqI37za3Wy2Vn
        ssTvHB9eU1bpRgcD6TOHSCQomRSZEZWMilq68e2AKF7nXzQn+3Am8m9A3z/9MvBO
        yHzZ+l5hAFaCDqt28y0HpOgGPHDiXG3bPm/q4XcdoxOLovxUA8CgLNTXBiU5Kyxe
        52chzGU8IZGBYKEmbL6z9i9Z8aQkz3JpLtrYP/Y/Mm1m9qsf/064r12lxgGlXFPX
        BQphaGDJfmLTak3BVoqG+PG2xuiZ6qJBQnFtoECOXbgXUxeRqmZce+bxnzaGa7j7
        Wxs3B0/Z7H2rKp19ju5EYMJUyHmkMmHRQUMru2aO50J08/Fco1HiUeegd2NlLTFU
        AofitEYQTtdL9A090KzPWyykDtRFq1K4PlGu887X7s3iueh1e0hlUYLLY36jvPBU
        ZoLaMZ6BBpUBVpLG5AUJ1GgBCQIQqXur9MJDuOzgoc4DrbmnPw80xOICoMfHk1Ds
        04JJxPYmcPOLgzOfgK7QsDB4I7XP/buZL3qDzetgephpRuSD5I41txJ/07Xqk1xy
        coKgUwQOhFmz2pAqvGOVMjjYgUkqk/bx6g==
        =tzN3
        -----END PGP MESSAGE-----
      fp: 781EE01BD4ACD521F47622CB7DAE7D1D01032CD8
    - created_at: '2025-02-17T11:33:11Z'
      enc: |-
        -----BEGIN PGP MESSAGE-----

        hQGMA6fLdw+f/UVQAQv/X5ddxsOXy3HNuZ09rzJ6VbRKx/hCGb9+kbyW1uCPH3HJ
        Klya6pIJ+6ZiibYXq0eZsH5a7Cp4Tukp342pOLOMk3KCF+b5S0SAEBfO8Q3ctl0n
        S2EKQf4ac4+XFyAh78ZvZhCugk71q4548ELB/MNu2mtkGIknFIjNcQ3ebbPfim1H
        ntSPnJHPpMQ75/d2wN9jM8LtwMw22wJMc8OAETtSB/FnTsFdGiit5+hNJQ5sVi62
        fYO5gk7tRKTn30xi7DeaNAuTLDjmwaocB+jabMWuF5TDPzhIUKn1pjXVPPPVrEFi
        vjoshEJ7UGuJI/6NdBhKTrxgFl282mLH+58TG28do8uhzMlDBsVMfLYgrt1F5aie
        3DTcvB8HxMY4n2bL9D47w7WQJY4dE+YP6XkpJkjogFdKZ8yYrRJ+qsY8ADPUXosU
        BNY55YY+Qmi0/UujfX/2bXFPRnXhNm+YedEBfXxkhRpWRbr9rs9EvzqGfpZmrvn2
        11YjTXVOovv6nZTdwnkr1GgBCQIQbLbCKT2FP2vNw1Qp/cuM7Cw0bCol/FlVKXys
        qW7a5NFAk9gEo3xNHA3k7HOAItAPOBd8EBoHh1fyVsCib5O9Kz/W27EHJzcgTJ0s
        2/KNDEYlW0DfFAjn5KExXo5FZk5JD1xLZQ==
        =954S
        -----END PGP MESSAGE-----
      fp: B9B825890A6505725A738356D3B4CAEFF79C866D
  unencrypted_suffix: _unencrypted
  version: 3.9.4
```

Please, note that the public keys are there and match Aaaron and Pedro PGP keys UID set on `.sops.yaml` file.

So when Pedro or Aaron run

```sh
# Decript the file
sops -d ./secrets/secrets.encrypted.yaml

# output
> api-key: some-api-key
```

### Proposed workflow

The proposed workflow is described here [`docs/secret-operations.md`](../../secret-operations.md)

## Decision

This ADR proposes the adoption of SOPS for secret management in local development. The implementation plan will be executed to roll out SOPS and train developers on its usage.

## Consequences

Developers will need to learn how to use SOPS and configure it with the chosen KMS (PGP for local development)

[cncf_sops]: https://www.cncf.io/projects/sops/
