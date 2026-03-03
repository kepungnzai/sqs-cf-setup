

# To create the stack 

aws cloudformation create-stack  --stack-name my-sqs-stack1  --template-body file://sqs.yaml  --region ap-southeast-2


# Create a new console app
dotnet new console -n SqsWriter

# Navigate to the project folder
cd SqsWriter

# Add the AWS SDK for SQS
dotnet add package AWSSDK.SQS --version 3.7.300.74

# (Optional) Add AWS configuration package
dotnet add package AWSSDK.Extensions.NETCore.Setup
