module.exports = function (grunt) {
    // Project configuration.
    grunt.initConfig({
        run: {
            options: {
                // Task-specific options go here.
            },
            buildCss: {
                exec: 'npm run buildCss'
            },
            buildAll: {
                exec: 'npm run buildAll'
            }
        },	
        pkg: grunt.file.readJSON('package.json'),
        dirs: {
            jsSrc: 'Scripts',
            cssSrc: 'Content/css',
            dest: 'Public',
            jsDest: '<%= dirs.dest %>/js',
            cssDest: '<%= dirs.dest %>/css',
            viewSrc: 'Content/css/Views',
            viewDest: 'Content/css/Views'
        },
        concat: {
            core: {
                files: {
                    '<%= dirs.jsDest %>/core.js': [
                        '<%= dirs.jsSrc %>/vendors/jquery.3.5.1.js',
                        '<%= dirs.jsSrc %>/vendors/jquery.migrate.1.2.1.min.js',
                        '<%= dirs.jsSrc %>/vendors/jquery.ui.autocomplete.1.12.1.js',
                        '<%= dirs.jsSrc %>/vendors/jquery.validate.js',
                        '<%= dirs.jsSrc %>/vendors/jquery.selectric.js'
                    ]
                }
            },
            vendors: {
                files: {
                    '<%= dirs.jsDest %>/vendors.js': [
                        '<%= dirs.jsSrc %>/vendors/bootstrap.js',
                        '<%= dirs.jsSrc %>/vendors/bootstrap.datepicker.js',
                        '<%= dirs.jsSrc %>/vendors/bootstrap.datepicker.tr.min.js',
                        '<%= dirs.jsSrc %>/vendors/device.js',
                        '<%= dirs.jsSrc %>/vendors/js.cookie.js',
                        '<%= dirs.jsSrc %>/vendors/lazysizes.js',
                        '<%= dirs.jsSrc %>/vendors/modernizr-2.8.3.js',
                        '<%= dirs.jsSrc %>/vendors/slick.min.js',
                        '<%= dirs.jsSrc %>/vendors/svg4everybody.js',
                        '<%= dirs.jsSrc %>/core/pointer.js',
                        '<%= dirs.jsSrc %>/core/core.js',
                        '<%= dirs.jsSrc %>/core/site.js',
                        '<%= dirs.jsSrc %>/views/Layout/Layout.js',
                        '<%= dirs.jsSrc %>/views/Content/owl.carousel.min.js',
                        '<%= dirs.jsSrc %>/views/Content/jquery.magnific-popup.min.js',
                        '<%= dirs.jsSrc %>/views/Content/smooth-scrollbar.js',
                        '<%= dirs.jsSrc %>/views/Content/select2.min.js',
                        '<%= dirs.jsSrc %>/views/Content/slider-radio.js',
                        '<%= dirs.jsSrc %>/views/Content/jquery.inputmask.min.js',
                        '<%= dirs.jsSrc %>/views/Content/plyr.min.js',
                        '<%= dirs.jsSrc %>/views/Content/main.js'
                    ]
                }
            },
            detail: {
                files: {
                    '<%= dirs.jsDest %>/product.js': [
                        '<%= dirs.jsSrc %>/views/Product/easyzoom.js',
                        '<%= dirs.jsSrc %>/views/Product/jquery-smartphoto.js'
                    ]
                }
            }
        },
        uglify: {
            dist: {
                files: {
                    '<%= dirs.jsDest %>/core.min.js': ['<%= dirs.jsDest %>/core.js'],
                    '<%= dirs.jsDest %>/vendors.min.js': ['<%= dirs.jsDest %>/vendors.js'],
                    '<%= dirs.jsDest %>/product.min.js': ['<%= dirs.jsDest %>/product.js'],
                    '<%= dirs.jsDest %>/register.min.js': ['<%= dirs.jsSrc %>/views/Member/Register.js'],
                    '<%= dirs.jsDest %>/login.min.js': ['<%= dirs.jsSrc %>/views/Member/Login.js'],
                    '<%= dirs.jsDest %>/forget.password.min.js': ['<%= dirs.jsSrc %>/views/Member/Forget.Password.js'],
                    '<%= dirs.jsDest %>/personel.information.min.js': ['<%= dirs.jsSrc %>/views/Member/Personel.Information.js'],
                    '<%= dirs.jsDest %>/comments.min.js': ['<%= dirs.jsSrc %>/views/Member/Comments.js']
                }
            }
        },
        sass: {
            dist: {
                options: {
                    style: 'expanded'
                },
                files: {
                    '<%= dirs.cssDest %>/style.css': '<%= dirs.cssSrc %>/style.scss'
                }
            }
        },
        cssmin: {
            options: {
                mergeIntoShorthands: false,
                roundingPrecision: -1,
                rebase: true
            },
            dist: {
                files: {
                    '<%= dirs.cssDest %>/style.min.css': ['<%= dirs.cssDest %>/style.css']
                }
            }
        },
        watch: {
            scripts: {
                files: ['**/*.js'],
                tasks: ['buildjs'],
                options: {
                    spawn: false,
                },
            },
            css: {
                files: ['**/*.scss'],
                tasks: ['buildcss'],
                options: {
                    spawn: false,
                },
            }
        },
        svgstore: {
            options: {
                prefix: 'ui-', // This will prefix each ID
                cleanup: true,
                includeTitleElement: false,
                inheritviewbox: true
            },
            default: {
                files: {
                    'Public/svg/icons.svg': ['Content/icons/optimize/*.svg']
                },
            },
        },
        svgmin: {
            options: {
                plugins: [
                    {
                        removeViewBox: false
                    },
                    {
                        removeUselessStrokeAndFill: false
                    },
                    {
                        removeAttrs: {
                            attrs: [
                                'xmlns'
                            ]
                        }
                    }
                ]
            },
            dist: {
                files: [
                    {
                        expand: true,     // Enable dynamic expansion.
                        cwd: 'Content/icons/',      // Src matches are relative to this path.
                        src: '*.svg', // Actual pattern(s) to match.
                        dest: 'Content/icons/optimize',   // Destination path prefix.
                        ext: '.svg',   // Dest filepaths will have this extension.
                        extDot: 'first'   // Extensions in filenames begin after the first dot
                    },
                ]
            }
        },
        critical: {
            homepage: {
                options: {
                    base: './',
                    css: ['<%= dirs.cssDest %>/style.min.css'],
                    width: 1920,
                    height: 1080
                },
                src: 'http://localhost:3960/',
                dest: '<%= dirs.cssDest %>/critical/home.min.css'
            },
            //member: {
            //    options: {
            //        base: './',
            //        css: ['<%= dirs.cssDest %>/style.min.css'],
            //        width: 1920,
            //        height: 1080
            //    },
            //    src: 'http://localhost:3960/Member/Index',
            //    dest: '<%= dirs.cssDest %>/critical/member.min.css'
            //}
        }
    });
    grunt.loadNpmTasks('grunt-run');
    // Load the plugin that provides the "uglify" task.
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-sass');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-svgstore');
    grunt.loadNpmTasks('grunt-svgmin');
    grunt.loadNpmTasks('grunt-critical');
    // Default task(s).
    grunt.registerTask('createandbuildcss', ['run:buildCss']);
    grunt.registerTask('css', ['sass', 'cssmin']);
    grunt.registerTask('js', ['concat:core', 'concat:vendors', 'concat:detail', 'uglify']);
    grunt.registerTask('svg', ['svgmin', 'svgstore']);
    grunt.registerTask('createandbuildall', ['run:buildAll']);
    grunt.registerTask('all', ['concat', 'uglify', 'sass', 'cssmin', 'svgmin', 'svgstore']);
    grunt.registerTask('criticalCss', ['critical']);
    grunt.registerTask('watch', ['watch']);
    grunt.registerTask('wcss', ['watch:css']);
    grunt.registerTask('wjs', ['watch:scripts']);
    grunt.registerTask('default', ['concat', 'uglify', 'sass', 'cssmin', 'svgmin', 'svgstore']);
};