const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = {
    entry: {
        app: './src/stress/app.js'
    },
    plugins: [
        new CleanWebpackPlugin()
    ],
    output: {
        filename: 'stress.bundle.js',
        path: path.resolve(__dirname, './wwwroot/js/'),
    },
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    "style-loader",
                    {
                        loader: "file-loader",
                        options: {
                            name: "../styles/[name].css"
                        }
                    },
                    "sass-loader"
                ]
            }
        ]
    }
};