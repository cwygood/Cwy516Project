<template>
    <div>
        <user ref="user" @ShowAllUser="GetAllUser"></user>
        <el-form ref='form' :model="form">
            <el-form-item label="用户id" prop="userId">
                <el-input v-model="form.userId" style="width:10%" placeholder="用户id"></el-input>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="GetAllUser">获取所有用户</el-button>
                <el-button type="primary" @click="GetUser(form.userId)">获取单个用户</el-button>
                <el-button type="primary" @click="AddUser">新增用户</el-button>
            </el-form-item>
        </el-form>
        <el-table :data="userInfos">
            <el-table-column prop="id" label="ID">
            </el-table-column>
            <el-table-column prop="name" label="名称">
                <template slot-scope="scope">
                    <span v-if="scope.row.isSet">
                        <el-input size="mini" placeholder="请输入内容" v-model="scope.row['name']">
                        </el-input>
                    </span>
                    <span v-else>{{scope.row['name']}}</span>
                </template>
            </el-table-column>
            <el-table-column prop="code" label="编码">
            </el-table-column>
            <el-table-column prop="status" label="状态">
                <template slot-scope="scope">
                    <span v-if="scope.row.isSet">
                        <el-select v-model="scope.row.status" placeholder="请选择">
                            <el-option v-for="item in userStatus" :key="item.id" :label="item.name" :value="item.id">
                            </el-option>
                        </el-select>
                    </span>
                    <span v-else>{{userStatus[scope.row['status']].name}}</span>
                </template>
            </el-table-column>
            <el-table-column label="操作">
                <template slot-scope="scope">
                    <el-button size="small" @click="Edit(scope.$index,scope.row)">编辑</el-button>
                    <el-button size="small" @click="Delete(scope.$index,scope.row)">删除</el-button>
                </template>
            </el-table-column>
        </el-table>
    </div>
    
</template>

<script>
import User from "../components/user/user.vue"
export default{
    components:{User},
    data() {
        return {
            form:{
                userId:""
            },
            userInfos:[],
            userStatus:[
                {id:0,name:"正常"},
                {id:1,name:"禁用"},
                {id:2,name:"锁定"}
            ]
        }
    },
    created(){
        this.GetAllUser();
    },
    methods:{
        GetAllUser(){
            this.$request.post('home/GetAllUser',{requestId : ""}).then(response=>{
                this.userInfos=[];
                response.data.forEach(element => {
                    // this.userInfos.push({
                    //     id:element.id,
                    //     name:element.name,
                    //     code:element.code,
                    //     status:element.status,
                    //     isSet:false
                    // });
                    this.userInfos.push(Object.assign({},element,{isSet:false}));
                });
                //this.userInfos=response.data;
            })
        },
        GetUser(userId){
            this.$request.post('home/GetUserById',{requestId : "", id : userId}).then(respone=>{
                this.userInfos= [];
                // this.userInfos.push({
                //     id:respone.data.id,
                //     name:respone.data.name,
                //     code:respone.data.code,
                //     status:respone.data.status,
                //     isSet:false
                // });
                this.userInfos.push(Object.assign({},respone.data,{isSet:false}));
            })
        },
        Edit(index,row){
            row.isSet = true;
        },
        Delete(index,row){
            this.userInfos.splice(index,1);
        },
        AddUser(form){
            this.$refs.user.Show();
            // this.$request.post('home/AddUser',{
            //     requestId : "",
            //     userName : form.userName,

            // })
        }
    }
}
</script>
