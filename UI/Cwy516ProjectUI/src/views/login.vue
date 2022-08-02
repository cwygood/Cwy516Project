<template>
  <div class="login-container">
    <el-form ref="form" :rules="rules" :model="form" label-width="80px" class="login-form">
      <el-form-item label="用户名" prop="userName">
        <el-input v-model="form.userName" placeholder="用户名"></el-input>
      </el-form-item>
      <el-form-item label="密码" prop="password">
        <el-input type="password" v-model="form.password" placeholder="密码"></el-input>
      </el-form-item>
      <el-form-item label="验证码" prop="validateCode">
        <el-input v-model="form.validateCode" style="width: 70%;float: left" placeholder="验证码" @keyup.enter.native="submitForm"></el-input>
        <div style="float:left">
          <img v-bind:src="imageUrl" border="1" style="margin: 2px 0 0 5px;height:70%" @click="refreshCode"/>
        </div>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm">登录</el-button>
        <el-button type="primary" @click="resetForm">重置</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import { encrypt, decrypt, generatekey } from '@/utils/aes.js'
    export default{
        data(){
            return {
                form:{
                    userName:"",
                    password:"",
                    validateCode:"",
                    validateKey:""
                },
                imageUrl:this.getValidateCode(),
                validatekey:'',
                rules:{
                    userName:[
                        {required:true,message:"用户名不能为空",trigger:'blur'},
                        {min:3,max:10,message:"用户名3-10位",trigger:'blur'}
                    ],
                    password:[
                        {required:true,message:"密码不能为空",trigger:'blur'},
                        {min:3,max:10,message:"密码3-10位",trigger:'blur'}
                    ],
                    validateCode:[
                        {required:true,message:"验证码不能为空",trigger:'blur'}
                    ]
                }
            }
        },
        methods:{
            submitForm(){
                this.$refs.form.validate(valid=>{
                    if(valid){
                      this.form.validateKey=this.validateKey;
                      this.$request({method:'post',url: 'Home/Login',data:this.form})
                      .then(response=>{
                        this.$storage.set('token', response.data.token);
                        this.$storage.set('userToken', response.data.userToken);
                        this.$router.push('/index')
                      }).catch(error=>{
                        this.refreshCode();
                        console.log(error);
                      });
                    }
                    else{
                      this.refreshCode();
                      console.log('验证失败');
                      return false;
                    }
                });
            },
            resetForm(){
              this.$refs.form.resetFields();
            },
            getValidateCode(){
              this.validateKey = generatekey(8);
              return window.API_URL.mainUrl + 'Home/CreateValidateCode?validateKey=' + this.validateKey;
            },
            refreshCode(){
              this.imageUrl = this.getValidateCode();
            }
        }
    }

</script>

<style lang="scss" scoped>
.login-form {
  width: 350px;
  margin: 160px auto; /* 上下间距160px，左右自动居中*/
  background-color: rgb(255, 255, 255, 0.8); /* 透明背景色 */
  padding: 30px;
  border-radius: 20px; /* 圆角 */
}

/* 背景 */
.login-container {
  position: absolute;
  width: 100%;
  height: 100%;
  background: #72bbec //url("../../assets/logo.png");
}

/* 标题 */
.login-title {
  color: #303133;
  text-align: center;
}
</style>
