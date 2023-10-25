#!/usr/bin/env bash
        
     set -euo pipefail
        
     # enable debug
     # set -x
      
        
     echo "configuring sns/sqs"
     echo "==================="
     # https://gugsrs.com/localstack-sqs-sns/
     LOCALSTACK_HOST=localhost
     AWS_REGION=us-east-1
     LOCALSTACK_DUMMY_ID=000000000000
     QUEUE_NAME_TO_CREATE=dummy-queue.fifo
        
     get_all_queues() {
         awslocal --endpoint-url=http://${LOCALSTACK_HOST}:4566 sqs list-queues
     }
        
        
     create_queue() {
         awslocal --endpoint-url=http://${LOCALSTACK_HOST}:4566 sqs create-queue --attributes FifoQueue=true --queue-name ${QUEUE_NAME_TO_CREATE}
     }
        
        
     guess_queue_arn_from_name() {
         echo "arn:aws:sqs:${AWS_REGION}:${LOCALSTACK_DUMMY_ID}:$QUEUE_NAME_TO_CREATE"
     }
        
     echo "creating queue $QUEUE_NAME_TO_CREATE"
     QUEUE_URL=$(create_queue ${QUEUE_NAME_TO_CREATE})
     echo "created queue: $QUEUE_URL"
     QUEUE_ARN=$(guess_queue_arn_from_name $QUEUE_NAME_TO_CREATE)
        
     echo "all queues are:"
     echo "$(get_all_queues)"
