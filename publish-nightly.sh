#!/usr/bin/env bash

setup_git() {
    git config --global user.email "travis@travis-ci.org"
    git config --global user.name "Travis CI - Bot"
}

update_nightly_branch() {
    git remote add origin-nightly https://${GH_TOKEN}@github.com/ashblue/fluid-behavior-tree.git
    git push origin-nightly `git subtree split --prefix Assets/FluidBehaviorTree master`:nightly --force
}

setup_git
update_nightly_branch
