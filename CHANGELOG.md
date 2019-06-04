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
