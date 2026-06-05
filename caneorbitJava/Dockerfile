FROM eclipse-temurin:21-jdk-alpine AS build

WORKDIR /app

COPY pom.xml .
RUN apk add --no-cache mvnw && ./mvnw dependency:go-offline

COPY src src
RUN ./mvnw package -DskipTests

FROM eclipse-temurin:21-jdk-alpine

RUN addgroup -S caneorbitgroup && adduser -S caneorbituser -G caneorbitgroup

WORKDIR /app

COPY --from=build /app/target/*.jar app.jar

USER caneorbituser

EXPOSE 8080

ENTRYPOINT ["java", "-jar", "app.jar"]