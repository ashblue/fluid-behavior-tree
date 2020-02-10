## [2.2.1](https://github.com/ashblue/fluid-behavior-tree/compare/v2.2.0...v2.2.1) (2020-02-10)


### Bug Fixes

* **parallel:** prevents crashes when used with Splice() ([010d4ef](https://github.com/ashblue/fluid-behavior-tree/commit/010d4ef))

# [2.2.0](https://github.com/ashblue/fluid-behavior-tree/compare/v2.1.0...v2.2.0) (2019-06-13)


### Bug Fixes

* **travis:** nightly builds moved to a force push ([b1ae62d](https://github.com/ashblue/fluid-behavior-tree/commit/b1ae62d))
* nightly builds set to the wrong branch ([4413d51](https://github.com/ashblue/fluid-behavior-tree/commit/4413d51))
* **behavior-tree:** recursive global position changes don't double up ([e762e8e](https://github.com/ashblue/fluid-behavior-tree/commit/e762e8e))
* **visualizer:** bugfix for images vanishing ([53dcf0d](https://github.com/ashblue/fluid-behavior-tree/commit/53dcf0d))


### Features

* nightly builds now available ([2cd0774](https://github.com/ashblue/fluid-behavior-tree/commit/2cd0774))
* **behavior-tree:** runtime trees can now be visually printed ([11534c7](https://github.com/ashblue/fluid-behavior-tree/commit/11534c7))
* **decorators:** add three new decorators ([f376ea6](https://github.com/ashblue/fluid-behavior-tree/commit/f376ea6))


### Reverts

* **nightly:** removing nightly back to what it originally was ([f697eec](https://github.com/ashblue/fluid-behavior-tree/commit/f697eec))

# [2.1.0](https://github.com/ashblue/fluid-behavior-tree/compare/v2.0.1...v2.1.0) (2019-06-04)


### Bug Fixes

* **manifest:** moved to scoped registries ([19dd266](https://github.com/ashblue/fluid-behavior-tree/commit/19dd266))


### Features

* **behavior-tree:** calling `Reset()` now triggers End on active nodes ([0287fa8](https://github.com/ashblue/fluid-behavior-tree/commit/0287fa8))

## [2.0.1](https://github.com/ashblue/fluid-behavior-tree/compare/v2.0.0...v2.0.1) (2019-05-31)


### Bug Fixes

* missing meta files no longer crash the dist package ([32f782d](https://github.com/ashblue/fluid-behavior-tree/commit/32f782d))

# [2.0.0](https://github.com/ashblue/fluid-behavior-tree/compare/v1.0.0...v2.0.0) (2019-05-31)


### Bug Fixes

* **github:** fixed corrupt GitHub archive generation ([f94a140](https://github.com/ashblue/fluid-behavior-tree/commit/f94a140))
* **semantic-release:** prevents a crash due to no version number change ([55aa2ba](https://github.com/ashblue/fluid-behavior-tree/commit/55aa2ba))


### BREAKING CHANGES

* **github:** Artifical breaking change to make the number match with the correct version

# [2.0.0](https://github.com/ashblue/fluid-behavior-tree/compare/v1.0.0...v2.0.0) (2019-05-31)


### Bug Fixes

* **github:** fixed corrupt GitHub archive generation ([f94a140](https://github.com/ashblue/fluid-behavior-tree/commit/f94a140))


### BREAKING CHANGES

* **github:** Artifical breaking change to make the number match with the correct version

# 1.0.0 (2019-05-31)


### Bug Fixes

* **travis:** fixes an npm verification bug ([3bbb40c](https://github.com/ashblue/fluid-behavior-tree/commit/3bbb40c))


### Code Refactoring

* **namespace:** namespace changed to CleverCrow.Fluid.BTs ([073c1e9](https://github.com/ashblue/fluid-behavior-tree/commit/073c1e9))


### Features

* **action:** new wait time node ([bc49935](https://github.com/ashblue/fluid-behavior-tree/commit/bc49935))
* **commits:** added commitizen support ([916f5cb](https://github.com/ashblue/fluid-behavior-tree/commit/916f5cb))
* **composites:** new selector random node ([0e13b8d](https://github.com/ashblue/fluid-behavior-tree/commit/0e13b8d))


### BREAKING CHANGES

* **namespace:** Will cause any project using the old namespace to break
