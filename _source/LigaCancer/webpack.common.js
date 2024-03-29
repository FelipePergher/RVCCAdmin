﻿"use strict";
const path = require("path");
const CleanWebpackPlugin = require("clean-webpack-plugin");
const webpack = require("webpack");
//const glob = require("glob");
const FixStyleOnlyEntriesPlugin = require("webpack-fix-style-only-entries");
const StylelintPlugin = require("stylelint-webpack-plugin");
const ESLintPlugin = require("eslint-webpack-plugin");

module.exports = {
    entry: {
        "common": ["./src/sass/common/index.scss", "./src/scripts/common/index.js"],
        "home": ["./src/sass/home/index.scss", "./src/scripts/home/index.js"],
        "doctor": ["./src/sass/doctor/index.scss", "./src/scripts/doctor/index.js"],
        "treatment-place": ["./src/sass/treatment-place/index.scss", "./src/scripts/treatment-place/index.js"],
        "medicine": ["./src/sass/medicine/index.scss", "./src/scripts/medicine/index.js"],
        "cancer-type": ["./src/sass/cancer-type/index.scss", "./src/scripts/cancer-type/index.js"],
        "auxiliar-accessory": ["./src/sass/auxiliar-accessory/index.scss", "./src/scripts/auxiliar-accessory/index.js"],
        "expense-type": ["./src/sass/expense-type/index.scss", "./src/scripts/expense-type/index.js"],
        "treatment-type": ["./src/sass/treatment-type/index.scss", "./src/scripts/treatment-type/index.js"],
        "service-type": ["./src/sass/service-type/index.scss", "./src/scripts/service-type/index.js"],
        "patient": ["./src/sass/patient/index.scss", "./src/scripts/patient/index.js"],
        "patient-details": ["./src/sass/patient-details/index.scss", "./src/scripts/patient-details/index.js"],
        "patient-print": ["./src/sass/patient-print/index.scss"],
        "birthday": ["./src/sass/birthday/index.scss", "./src/scripts/birthday/index.js"],
        "user": ["./src/sass/user/index.scss", "./src/scripts/user/index.js"],
        "login": ["./src/sass/login/index.scss", "./src/scripts/login/index.js"],
        "reset-password": ["./src/sass/reset-password/index.scss", "./src/scripts/reset-password/index.js"],
        "forgot-password": ["./src/sass/forgot-password/index.scss", "./src/scripts/forgot-password/index.js"],
        "change-password": ["./src/sass/change-password/index.scss", "./src/scripts/change-password/index.js"],
        "resend-email": ["./src/sass/resend-email/index.scss", "./src/scripts/resend-email/index.js"],
        "benefit": ["./src/sass/benefit/index.scss", "./src/scripts/benefit/index.js"],
        "patient-benefit": ["./src/sass/patient-benefit/index.scss", "./src/scripts/patient-benefit/index.js"],
        "stay": ["./src/sass/stay/index.scss", "./src/scripts/stay/index.js"],
        "admin": ["./src/sass/admin/index.scss", "./src/scripts/admin/index.js"],
        "sale-shirt-2020": ["./src/sass/sale-shirt-2020/index.scss", "./src/scripts/sale-shirt-2020/index.js"],
        "visitor": ["./src/sass/visitor/index.scss", "./src/scripts/visitor/index.js"],
        "attendant": ["./src/sass/attendant/index.scss", "./src/scripts/attendant/index.js"],
        "attendance-type": ["./src/sass/attendance-type/index.scss", "./src/scripts/attendance-type/index.js"],
        "visitor-attendance-type": ["./src/sass/visitor-attendance-type/index.scss", "./src/scripts/visitor-attendance-type/index.js"]
    },
    plugins: [
        new CleanWebpackPlugin(),
        new webpack.ProvidePlugin({
            $: require.resolve("jquery"),
            jQuery: require.resolve("jquery"),
            "window.jQuery": "jquery"
            //Promise: ["es6-promise", "Promise"]
        }),
        new webpack.ProvidePlugin({
            moment: require.resolve("moment")
        }),
        new FixStyleOnlyEntriesPlugin(),
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
        new StylelintPlugin(),
        new ESLintPlugin()
    ],
    output: {
        filename: "js/[name].bundle.js",
        path: path.resolve(__dirname, "wwwroot/dist"),
        publicPath: "/"
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                loader: "ts-loader",
                exclude: /node_modules/
            },
            {
                test: /\.css$/,
                loader: "style-loader!css-loader",
                options: {
                    minimize: true
                }
            },
            {
                test: /\.js$/,
                loader: "babel-loader",
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
                    loader: "url-loader",
                    options: {
                        name: "fonts/[hash]-[name].[ext]"
                    }
                }]
            },
            {
                test: /\.(png|jp(e*)g|svg)$/,
                use: [{
                    loader: "url-loader",
                    options: {
                        limit: 8000, // Convert images < 8kb to base64 strings
                        name: "../img/[hash]-[name].[ext]"
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
                    name: "vendors",
                    priority: -10,
                    chunks: "all",
                    enforce: true
                },
                default: {
                    name: "commonChunks",
                    minChunks: 2,
                    priority: -20,
                    reuseExistingChunk: true
                }
            }
        }
    }

};