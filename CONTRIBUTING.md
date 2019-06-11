# Contributing Guidelines

Here are some general guidelines to please include in your PR to prevent potential revision requests.

* Make sure your code is formatted similar to the rest of the project
* New code must have test coverage. See any *Test.cs for an example

## Pull Requests

This project conforms to the Git Flow branching strategy. Please do the following if you wish to create a pull request.

1. Place your code in a branch named `feature/MY_PR_TITLE` that branches off of `develop`
1. Submit a pull request to the `develop` branch
    1. Verify all cloud checks pass without crashing
    1. Await a PR review for potential revisions or feedback
    1. Once the PR is approved it will be merged into `develop` and immediately available in the `nightly` branch

## Submitting New Tasks

If you want to submit new conditions, actions, decorators, or composites please keep the following in mind.

* Each task should be committed as an individual *feature* so it compiles to release notes properly
* Tasks should include a custom white icon (preferably from https://material.io/tools/icons/). See any composite task for a good example
* All tasks require integration with the tree builder script
* You must also include documentation in the `README.md` that demonstrates how to use the task and what it does
