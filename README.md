# Jay's getting started with FsLab

## How I set up this repository/app

1. Create a new F# console app:

    a. Create a new directory

    b. Run

        ```bash
        dotnet new console --language F#
        ```

    c. Copied the `global.json` and `.editorconfig` files from the Forest project to here. (And removed a few unnecessary fantomas settings, I dind't know those where still there in Forest actually...).

2. Initiate git

    a. Creat a local git repo with
       ```bash
        git init
        ```

    b. Go to github and follow the steps there to create a repository and set it up as a remote for this.
   
    c. Copied in the .gitignore from the Forest project and removed all the Forest-specific lines.

4. Install local dotnet tools

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

5. Add packages from the [FsLab Getting Started post](https://fslab.org/blog/posts/getting-started.html) (but using paket rather than opening in script).
    - FSharp.Data
    - Deedle
