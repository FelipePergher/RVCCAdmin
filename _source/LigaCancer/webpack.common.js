"use strict";
const path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const webpack = require('webpack');
const glob = require("glob");
const FixStyleOnlyEntriesPlugin = require("webpack-fix-style-only-entries");

module.exports = {
    entry: {
        'common': ['./src/sass/common/index.scss', './src/scripts/common/index.js'],
        'doctor': ['./src/sass/doctor/index.scss', './src/scripts/doctor/index.js']
    },
    plugins: [
        new CleanWebpackPlugin(),
        new webpack.ProvidePlugin({
            $: require.resolve('jquery'),
            jQuery: require.resolve('jquery'),
            "window.jQuery": "jquery"
            //Promise: ['es6-promise', 'Promise']
        }),
        new FixStyleOnlyEntriesPlugin(),
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/)
    ],
    output: {
        filename: 'js/[name].bundle.js',
        path: path.resolve(__dirname, 'wwwroot/dist'),
        publicPath: '/'
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                loader: 'ts-loader',
                exclude: /node_modules/
            },
            {
                test: /\.css$/,
                loader: "style-loader!css-loader",
                options: {
                    minimize: true || {/* or CSSNano Options */ }
                }
            },
            {
                test: /\.js$/,
                loader: 'babel-loader',
                options: {
                    "presets": [
                        [
                            "@babel/preset-env",
                            {
                                "modules": "commonjs",
                                "targets": {
                                    "node": "current",
                                    "ie": "11"
                                }
                            }
                        ]
                    ]
                }
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2)$/,
                use: [{
                    loader: 'url-loader',
                    options: {
                        name: 'fonts/[hash]-[name].[ext]'
                    }
                }]
            },
            {
                test: /\.(png|jp(e*)g|svg)$/,
                use: [{
                    loader: 'url-loader',
                    options: {
                        limit: 8000, // Convert images < 8kb to base64 strings
                        name: '../img/[hash]-[name].[ext]'
                    }
                }]
            }
        ]
    },
    resolve: {
        extensions: [".tsx", ".ts", ".js"]
    },
    optimization: {
        splitChunks: {
            chunks: "all",
            minSize: 0,
            cacheGroups: {
                vendors: {
                    test: /[\\/]node_modules[\\/]/,
                    name: 'vendors',
                    priority: -10,
                    chunks: "all",
                    enforce: true
                },
                default: {
                    name: 'commonChunks',
                    minChunks: 2,
                    priority: -20,
                    reuseExistingChunk: true
                }
            }
        }
    }

};