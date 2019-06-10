#!/usr/bin/env bash

setup_git() {
    git config --global user.email "travis@travis-ci.org"
    git config --global user.name "Travis CI - Bot"
}

update_nightly_branch() {
    printf "Current Git details \n"
    git rev-parse HEAD
    git branch | grep \* | cut -d ' ' -f2

    printf "Run nightly update \n"
    git remote add origin-nightly https://${GH_TOKEN}@github.com/ashblue/fluid-behavior-tree.git
    git subtree push --prefix Assets/FluidBehaviorTree origin-nightly nightly
}

setup_git
update_nightly_branch
