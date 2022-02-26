const merge = require('webpack-merge');
const common = require('./webpack.common.js');
const webpack = require('webpack');
const glob = require("glob");
const path = require('path');
const BundleAnalyzerPlugin = require("webpack-bundle-analyzer").BundleAnalyzerPlugin;

module.exports = merge(common, {
    mode: 'development',
    devtool: false,
    devServer: {
        hot: true
    },
    plugins: [
        new webpack.SourceMapDevToolPlugin({
            filename: 'js/[name].js.map'
        }),
        new BundleAnalyzerPlugin(),
    ], module: {
        rules: [{
            test: /\.scss$/,
            use: [
                {
                    loader: 'file-loader',
                    options: {
                        name: 'css/[folder].bundle.css'

                    }
                },
                {
                    loader: 'extract-loader'
                },
                {
                    loader: "css-loader",
                    options: {
                        minimize: false,
                        sourceMap: true

                    }
                },
                {
                    loader: "sass-loader",
                    options: {
                        includePaths: glob.sync('node_modules').map((d) => path.join(__dirname, d))
                    }
                }
            ]
        }]
    }
});
