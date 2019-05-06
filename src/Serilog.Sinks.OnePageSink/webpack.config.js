var path = require('path')
const VueLoaderPlugin = require('vue-loader/lib/plugin');
var HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
    mode: 'development',
    entry: './assets/js/main.js',
    output: {
        path: path.resolve(__dirname, './dist'),
        publicPath: '~/',
        filename: 'js/build.js'
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            }, {
                test: /\.css$/,
                use: [
                    'vue-style-loader',
                    'css-loader'
                ]
            }
        ]
    },
    plugins: [
	    new HtmlWebpackPlugin({
	    filename: '/index.html',
	    template: './assets/index.html',
	    inject: true
    }),
        new VueLoaderPlugin()
       
    ]
}