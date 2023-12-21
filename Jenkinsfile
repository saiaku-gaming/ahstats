properties([pipelineTriggers([githubPush()])])

pipeline {
    agent {
        node {
            label 'gungnir'
        }
    }
    stages {
        stage('Build'){
            steps {
                script {
                    docker.build("ahstats")
                }
            }
        }
        stage('Deploy'){
            steps {
                script {
                    sh 'docker-compose up -d'
                }
            }
        }
    }
}