#!/usr/bin/env bash

setup_git() {
    git config --global user.email "travis@travis-ci.org"
    git config --global user.name "Travis CI - Bot"
}

commit_files() {
    sed -i '/dist/d' ./.gitignore
    git add dist
    git commit -m "Update nightly files"
}

update_nightly_branch() {
    git remote add origin-nightly https://${GH_TOKEN}@github.com/ashblue/fluid-behavior-tree.git
    git subtree push --prefix dist origin-nightly nightly
}

setup_git
commit_files
update_nightly_branch
