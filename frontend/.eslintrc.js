module.exports = {
  extends: ['airbnb-typescript', 'tslint-react-hooks'],
  plugins: ['prettier'],
  rules: {
    'prettier/prettier': 'error',
    'react-hooks-nesting': 'error'
  },
  env: {
    browser: true,
    node: true,
    jest: true // here we write libraries that is connected
  },
  globals: {
    document: false
  }
};
