import React, { Component } from 'react'

class Login extends Component {
    constructor(props) {
        super(props);

        this.state = {
            userName: ''
        }

        this.onSubmit = this.onSubmit.bind(this);
        this.onChangeUserName = this.onChangeUserName.bind(this);
    }

    onSubmit(e) {
        e.preventDefault();

        alert(this.state.userName);
    }

    onChangeUserName(e) {
        this.setState({
            userName: e.target.value
        });
    }

    render() {
        return (
            <div>
                <form onSubmit={this.onSubmit}>
                    Enter you name:
                <input
                        id="user"
                        type="text"
                        value={this.state.userName}
                        onChange={this.onChangeUserName}
                        />
                    <input type="submit" value="Send" />
                </form>
            </div>
        )
    }
}

export default Login