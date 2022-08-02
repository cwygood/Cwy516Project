import axios from 'axios'
import { Message } from 'element-ui';
import storage from './storage';

const service = axios.create({
    headers: {
        'content-type': 'application/json;charset=UTF-8'
    },
    baseURL: window.API_URL.mainUrl,
    timeout: 10000,
    withCredentials: true
})

//添加请求拦截器
service.interceptors.request.use(config => {
    //发送请求前做些事情，比如设置请求头，传递参数
    var token = storage.get('token');
    if (token) {
        config.headers['Authorization'] = "Bearer " + token;
        config.headers["token"] = storage.get('userToken');
    }
    return config;
}, error => {
    //请求错误是做些事情
    return Promise.reject(error);
});

//添加响应拦截器
service.interceptors.response.use(response => {
    //对响应数据做些事
    if (response.status === 200) {
        console.log(response);
        if (response.data && response.data.code == 0) {
            console.log('Success');
            Message({
                type: 'success',
                message: JSON.stringify(response.data.messages)
            });
            return response.data;
        }
        else {
            Message({
                type: 'error',
                message: JSON.stringify(response.data.messages)
            });
        }
        return Promise.reject(response.data);
    }
    else {
        Message({
            type: 'error',
            message: "服务器错误"
        });
    }
    return response;
}, error => {
    if (error.response) {
        switch (error.response.status) {
            // 返回401，清除token信息并跳转到登录页面
            case 401:
                // store.dispatch('user/resetToken');
                // let timer = setTimeout(() => {
                //   window.location.reload();
                //   clearTimeout(timer);
                //   timer = null;
                // }, 1500);
                Message({
                    type: 'error',
                    message: "未授权"
                });
                break;
            // 请求参数错误
            case 400:
                if (error.response.data) {
                    var message = error.response.data.message || error.response.data.title;
                    Message({
                        type: 'error',
                        message: message
                    });
                } else {
                    Message({
                        type: 'error',
                        message: '请求参数错误！'
                    });
                }
                break;
            // 服务器错误
            default:
                Message({
                    type: 'error',
                    message: '服务器错误！'
                });
                break;
        }
        // 返回接口返回的错误信息
        return Promise.reject(error.response.data);
    }
    Message({
        type: 'error',
        message: "服务器错误" + error
    });
    return Promise.reject(error);
})

export default service;

