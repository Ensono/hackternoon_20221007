#!/bin/bash

template="deploy/k8s/slacktistics.yml"

vars="$(<config.local)"

eval "$vars"

kubeyaml="$(envsubst -i ${template} -no-unset -no-empty)"

echo "${kubeyaml}" | kubectl apply -f -
