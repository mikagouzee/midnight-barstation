pipeline {
    agent any

    stages {
        stage('Clone Repository') {
            steps {
                // Pull the latest code from GitHub
                git branch: 'main', url: 'https://github.com/mikagouzee/midnight-barstation'
            }
        }

        stage('Build .NET App') {
            steps {
                // Build the .NET app
                sh 'dotnet publish -c Release -o ./publish'
            }
        }

        stage('Build Docker Image') {
            steps {
                // Build the Docker image for the app
                sh 'docker build -t mikagouzee/midnight-barstation:latest .'
            }
        }

        stage('Push Docker Image') {
            steps {
                // Log in to DockerHub (ensure credentials are configured in Jenkins)
                withDockerRegistry([credentialsId: 'dockerhub-credentials', url: '']) {
                    // Push the Docker image
                    sh 'docker push docmika10/midnight-barstation:latest'
                }
            }
        }

        stage('Deploy to K3s') {
            steps {
                // Apply the Kubernetes manifest to deploy to the cluster
                sh 'kubectl apply -f deployment.yaml'
            }
        }
    }
}