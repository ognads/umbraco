const path = require('path');
var webpack = require('webpack');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');//For Js minification
const MiniCssExtractPlugin = require("mini-css-extract-plugin");//For Css extraction
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");//For Css minification


const isDev = process.env.NODE_ENV === 'development';

module.exports = {
    entry:  "./src/master",
    optimization: {
        minimizer: [
            new UglifyJsPlugin({
                cache: true,
                parallel: true,
                sourceMap: false // set to true if you want JS source maps
            }),
            new OptimizeCSSAssetsPlugin({})
        ]
    },
    watch: true,
    module:{
        rules:[
            {
                test:/\.css$/,
                use: [
                    MiniCssExtractPlugin.loader,// CSS extraction
                    'css-loader'// translates CSS into CommonJS
                ]
            },
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader, // CSS extraction
                    "css-loader", // generate string from css file
                    "sass-loader" // compiles Sass to CSS
                ]
            },
            //Below loaders are for managing url strings in files that we want to bundle
            {
                test: /\.(otf|gif|png|jpg|JPG)$/,
                loader: "url-loader"
            },
            {
                test: /\.woff(\?v=\d+\.\d+\.\d+)?$/,
                loader: "url-loader?limit=10000&mimetype=application/font-woff"
            }, {
                test: /\.woff2(\?v=\d+\.\d+\.\d+)?$/,
                loader: "url-loader?limit=10000&mimetype=application/font-woff"
            }, {
                test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/,
                loader: "url-loader?limit=10000&mimetype=application/octet-stream"
            }, {
                test: /\.eot(\?v=\d+\.\d+\.\d+)?$/,
                loader: "file-loader"
            }, {
                test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
                loader: "url-loader?limit=10000&mimetype=image/svg+xml"
            }
        ]
    },
    watch: true,
    resolve: {
        alias: {
            "jquery.validation": "jquery-validation/dist/jquery.validate.js" 
        }
    }, 
    plugins: [
        // Plugins which should always be executed
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery"
        }),
        new UglifyJsPlugin(),
        new MiniCssExtractPlugin({
            filename: "[name].css",
            chunkFilename: "[id].css"
        })

    ].concat(isDev ?
        [
            // Only exectued in dev environment
        ] :
        [
            // Only exectued in prod environment
        ]
    )
};
