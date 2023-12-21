properties([pipelineTriggers([githubPush()])])

pipeline {
    agent {
        node {
            label 'gungnir'
        }
    }
    stages {
        stage('Build') {
            steps {
                script {
                    docker.build("ahstats")
                }
            }
        }
        stage('Deploy') {
            environment {
                WowClient__ClientId=credentials('wowClientId')
                WowClient__ClientSecret=credentials('wowClientSecret')
            }
            steps {
                script {
                    sh 'docker-compose up -d'
                }
            }
        }
    }
}