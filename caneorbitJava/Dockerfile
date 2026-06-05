FROM eclipse-temurin:21-jdk-alpine

RUN addgroup -S caneorbitgroup && adduser -S caneorbituser -G caneorbitgroup

WORKDIR /app

COPY target/*.jar caneorbit-api.jar

USER caneorbituser

EXPOSE 8080

ENTRYPOINT ["java", "-jar", "caneorbit-api.jar"]