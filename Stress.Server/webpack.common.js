const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = {
    entry: {
        app: './src/stress/app.js',
    },
    plugins: [
        // new CleanWebpackPlugin(['dist/*']) for < v2 versions of CleanWebpackPlugin
        new CleanWebpackPlugin()
    ],
    output: {
        filename: 'stress.bundle.js',
        path: path.resolve(__dirname, './wwwroot/js/'),
    },
};