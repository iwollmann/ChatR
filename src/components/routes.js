import React from 'react';
import { Router, Route, browserHistory } from 'react-router';

import Login from './login/Login';

const Routes = () => (
  <Router history={browserHistory}>
    <Route path="/" component={Login} />
  </Router>
);

export default Routes;
