import React from 'react';
import { withNamespaces } from 'react-i18next';
import { connectTo } from '../../utils';
import Page from '../page';
import { resetPassword } from '../../actions/auth';

class ResetPassPage extends React.Component {
  state = {
  };

  componentDidMount() {
    !this.props.token && this.props.history.replace('/');
  }

  componentDidUpdate() {
    !this.props.token && this.props.history.replace('/');
  }

  render() {
    const { t } = this.props;
    return (
      <Page>
       
      </Page>
    );
  }
}

export default connectTo(
  state => ({
    token: state.auth.token
  }),
  {
    resetPassword
  },
  withNamespaces("account")(ResetPassPage)
);
