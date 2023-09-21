# Jay's getting started with FsLab

## What have I done in this repo so far

1. Create a new F# console app:

    a. Create a new directory

    b. Run

        ```bash
        dotnet new console --language F#
        ```

    c. Copied the `global.json` and `.editorconfig` files from the Forest project to here. (And removed a few unnecessary fantomas settings, I dind't know those where still there in Forest actually...).

2. Initiate git

        ```bash
        git init
        ```

    And then create a repo on github and link to this.
    Copied in the .gitignore from the Forest project and removed all the Forest-specific lines.

3. Install local dotnet tools

    a. Create a new tool manifest with

        ```bash
        dotnet new tool-manifest
        ```

    b. Add paket

        ```bash
        dotnet tool install paket
        ```

    c. Add fantomas

        ```bash
        dotnet tool install fantomas
        ```

4. Add packages from the [FsLab Getting Started post](https://fslab.org/blog/posts/getting-started.html) (but using paket rather than opening in script).
    - FSharp.Data
    - Deedle
