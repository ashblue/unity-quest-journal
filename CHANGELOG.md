# [2.0.0](https://github.com/ashblue/unity-quest-journal/compare/v1.0.1...v2.0.0) (2024-03-27)


### Bug Fixes

* **quest inspector:** cleaned up display bugs in the task display ([cc23082](https://github.com/ashblue/unity-quest-journal/commit/cc230826a167ce0b5e5d9b0c0a41baec313cbae4))


### Features

* **quest database:** quests are now separate ScriptableObjects independent of the database ([9c64eb5](https://github.com/ashblue/unity-quest-journal/commit/9c64eb5f200b34cd93f132e841150c16067b16e9))
* **quest events:** quest collection now has event hooks ([c535109](https://github.com/ashblue/unity-quest-journal/commit/c5351092340b6a538d170834217822185b94b141))
* **quests and tasks:** default quest and task definition can now be excluded from inspectors ([359ba7b](https://github.com/ashblue/unity-quest-journal/commit/359ba7be8fa19cc44493e601c26622d3b8c1b2a1))


### BREAKING CHANGES

* **quest database:** Breaks all existing quest implementations. It is recommended to not upgrade to this
version if you're on 1.X.

## [1.0.1](https://github.com/ashblue/unity-quest-journal/compare/v1.0.0...v1.0.1) (2021-11-28)


### Bug Fixes

* **builds:** prevents a crash by removing accidentally included editor only code ([b163656](https://github.com/ashblue/unity-quest-journal/commit/b16365679b7a23c848271325e8a5e6d90a07f016))

# 1.0.0 (2021-09-03)


### Features

* initial commit ([d1bfeef](https://github.com/ashblue/unity-quest-journal/commit/d1bfeef78371bb6486938e3c72a21d3a4fc30303))
